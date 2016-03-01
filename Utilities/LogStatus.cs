namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define um objecto que permite fazer o registo de diários.
    /// </summary>
    /// <typeparam name="Log">O tipo de objectos que constituem as entradas do diário.</typeparam>
    /// <typeparam name="Level">O tipo de objectos que constituem os níveis das entradas do diário.</typeparam>
    public interface ILogStatus<Log, Level>
    {
        /// <summary>
        /// Obtém um valor que indica se existem diários registados.
        /// </summary>
        /// <returns>Verdadeiro caso existam diários e falso caso contrário.</returns>
        bool HasLogs();

        /// <summary>
        /// Obtém um valor que indica se um diário existe para o nível especificado.
        /// </summary>
        /// <param name="level">O nível.</param>
        /// <returns>
        /// Verdadeiro caso o diário exista para o nível especificado e falso caso contrário.
        /// </returns>
        bool HasLogs(Level level);

        /// <summary>
        /// Adiciona um registo de diário ao estado.
        /// </summary>
        /// <param name="log">O registo de diário.</param>
        /// <param name="level">O nível do registo.</param>
        void AddLog(Log log, Level level);

        /// <summary>
        /// Obtém o enumerável para os diários.
        /// </summary>
        /// <returns>O par com os objectos que idenfiticam o nível e o diário associado.</returns>
        IEnumerable<KeyValuePair<Level, Log>> GetLogs();

        /// <summary>
        /// Obtém os diários associados a um nível específico.
        /// </summary>
        /// <param name="level">O nível.</param>
        /// <returns>O enumerável com os diários.</returns>
        IEnumerable<Log> GetLogs(Level level);

        /// <summary>
        /// Copia todos os erros registados do diário proporcionado.
        /// </summary>
        /// <param name="other">O diário do qual se pretende copiar os erros.</param>
        void CopyFrom(ILogStatus<Log, Level> other);
    }

    /// <summary>
    /// Documenta o estado de execução de um leitor de símbolos.
    /// </summary>
    /// <typeparam name="Log">O tipo de objectos que constituem os diários.</typeparam>
    /// <typeparam name="Level">O tipo de objectos que constituem os níveis dos diários.</typeparam>
    public class LogStatus<Log, Level> : ILogStatus<Log, Level>, ICloneable
    {
        /// <summary>
        /// Mantém o registo dos diários.
        /// </summary>
        private Dictionary<Level, List<Log>> logs;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LogStatus{Log, Level}"/>.
        /// </summary>
        public LogStatus()
        {
            this.logs = new Dictionary<Level, List<Log>>();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LogStatus{Log, Level}"/>.
        /// </summary>
        /// <param name="levelComparer">O comparador de níveis.</param>
        public LogStatus(IEqualityComparer<Level> levelComparer)
        {
            if (levelComparer == null)
            {
                throw new ArgumentNullException("levelComparer");
            }
            else
            {
                this.logs = new Dictionary<Level, List<Log>>(levelComparer);
            }
        }

        /// <summary>
        /// Obtém um valor que indica se existem diários registados.
        /// </summary>
        /// <returns>Verdadeiro caso existam diários e falso caso contrário.</returns>
        public bool HasLogs()
        {
            return this.logs.Count > 0;
        }

        /// <summary>
        /// Obtém um valor que indica se um diário existe para o nível especificado.
        /// </summary>
        /// <param name="level">O nível.</param>
        /// <returns>
        /// Verdadeiro caso o diário exista para o nível especificado e falso caso contrário.
        /// </returns>
        public bool HasLogs(Level level)
        {
            return this.logs.ContainsKey(level);
        }

        /// <summary>
        /// Adiciona um registo de diário ao estado.
        /// </summary>
        /// <param name="log">O registo de diário.</param>
        /// <param name="level">O nível do registo.</param>
        public void AddLog(Log log, Level level)
        {
            var adding = default(List<Log>);
            if (this.logs.TryGetValue(level, out adding))
            {
                adding.Add(log);
            }
            else
            {
                this.logs.Add(level, new List<Log>() { log });
            }
        }

        /// <summary>
        /// Copia todos os erros registados do diário proporcionado.
        /// </summary>
        /// <param name="other">O diário do qual se pretende copiar os erros.</param>
        public void CopyFrom(ILogStatus<Log, Level> other)
        {
            if (other != null)
            {
                var otherEnum = other.GetLogs().GetEnumerator();
                if (otherEnum.MoveNext())
                {
                    // Início da cópia
                    var previousLevel = otherEnum.Current.Key;
                    var messageList = default(List<Log>);
                    if (!this.logs.TryGetValue(previousLevel, out messageList))
                    {
                        messageList = new List<Log>();
                        this.logs.Add(previousLevel, messageList);
                    }

                    messageList.Add(otherEnum.Current.Value);

                    // Restantes elementos
                    while (otherEnum.MoveNext())
                    {
                        var currentLevel = otherEnum.Current.Key;
                        if (this.logs.Comparer.Equals(
                            previousLevel,
                            currentLevel))
                        {
                            messageList.Add(otherEnum.Current.Value);
                        }
                        else
                        {
                            // O nível poderá existir antes do início da cópia
                            if (!this.logs.TryGetValue(currentLevel, out messageList))
                            {
                                messageList = new List<Log>();
                                this.logs.Add(currentLevel, messageList);
                            }

                            messageList.Add(otherEnum.Current.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o enumerável para os diários.
        /// </summary>
        /// <returns>O par com os objectos que idenfiticam o nível e o diário associado.</returns>
        public IEnumerable<KeyValuePair<Level, Log>> GetLogs()
        {
            foreach (var kvp in this.logs)
            {
                foreach (var item in kvp.Value)
                {
                    yield return new KeyValuePair<Level, Log>(
                        kvp.Key,
                        item);
                }
            }
        }

        /// <summary>
        /// Obtém os diários associados a um nível específico.
        /// </summary>
        /// <param name="level">O nível.</param>
        /// <returns>O enumerável com os diários.</returns>
        public IEnumerable<Log> GetLogs(Level level)
        {
            var adding = default(List<Log>);
            if (this.logs.TryGetValue(level, out adding))
            {
                return adding;
            }
            else
            {
                return Enumerable.Empty<Log>();
            }
        }

        /// <summary>
        /// Limpa os diários registados.
        /// </summary>
        public void ClearLogs()
        {
            this.logs.Clear();
        }

        /// <summary>
        /// Limpa os diários associados ao nível especificado.
        /// </summary>
        /// <param name="level">O nível.</param>
        public void ClearLogs(Level level)
        {
            this.logs.Remove(level);
        }

        /// <summary>
        /// Obtém uma cópia do objecto corrente.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var result = new LogStatus<Log, Level>();
            result.logs = new Dictionary<Level, List<Log>>(
                this.logs.Comparer);
            foreach (var kvp in this.logs)
            {
                result.logs.Add(kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}
