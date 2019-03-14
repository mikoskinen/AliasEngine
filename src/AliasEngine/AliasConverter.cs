using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace AliasEngine
{
    public class AliasConverter
    {
        private readonly IAliasStore _aliasStore;
        private readonly ILogger<AliasConverter> _logger;

        public AliasConverter(IAliasStore aliasStore, ILogger<AliasConverter> logger)
        {
            _aliasStore = aliasStore;
            _logger = logger;
        }

        public string[] Convert(string command)
        {
            try
            {
                var result = new List<string>();

                DoConvert(command, ref result);

                return result.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to convert command to alias");
                Debug.WriteLine(ex.ToString());
                return new[] { command };
            }
        }

        private void DoConvert(string originalMessage, ref List<string> result, string parameters = "")
        {
            if (string.IsNullOrEmpty(originalMessage))
            {
                result.Add(string.Empty);
                return;
            }

            originalMessage = originalMessage.Trim();

            var words = originalMessage.Split(' ');
            var firstWord = words.First();

            var isKnownAlias = _aliasStore.ContainsAlias(firstWord);
            if (!isKnownAlias)
            {
                if (!string.IsNullOrEmpty(parameters))
                {
                    object[] stringParams1 = parameters.Split(' ').ToArray();
                    originalMessage = string.Format(originalMessage, stringParams1);
                }
                result.Add(originalMessage);
                return;
            }

            var myAlias = _aliasStore.GetAlias(firstWord);

            var containsMultipleCommands = myAlias.Contains("|");
            if (containsMultipleCommands)
            {
                var commands = myAlias.Split('|').Select(x => x.Trim());
                var param = string.Join(" ", words.Skip(1).ToArray());

                foreach (var command in commands)
                {
                    DoConvert(command, ref result, param);
                }

                return;
            }

            var parameterCountInAlias = myAlias.ToCharArray().Where(x => x == '{').Count();
            object[] stringParams = words.Skip(1).ToList().ToArray();
            if (stringParams.Length < parameterCountInAlias)
            {
                var allParams = new List<string>((IEnumerable<string>)stringParams);
                var emptyParamsNeeded = parameterCountInAlias - stringParams.Length;

                for (var i = 0; i < emptyParamsNeeded; i++)
                {
                    allParams.Add("");
                }

                stringParams = allParams.ToArray();
            }

            var outPut = string.Format(myAlias, stringParams);

            var shouldIncludeAllWordsFromTheOriginalMessage = outPut.ToCharArray().Last() == '-';
            if (shouldIncludeAllWordsFromTheOriginalMessage)
            {
                var skipCount = 1 + parameterCountInAlias;
                var restOfMessage = string.Join(" ", words.Skip(skipCount).ToArray());

                outPut = outPut.Remove(outPut.Length - 1) + " " + restOfMessage;
            }

            if (!string.Equals(outPut, originalMessage))
            {
                DoConvert(outPut, ref result);
            }
            else
            {
                result.Add(outPut);
            }

            return;
        }

        public void AddAlias(string newAlias)
        {
            try
            {
                _aliasStore.AddAlias(newAlias);
            }
            catch (Exception ex)
            {
                throw new FaultyAliasException(ex);
            }
        }

        public void InitialSetup()
        {
            AddAlias("/j /join {0}");
            AddAlias("/j2 /join {0} | /join {1}");
            AddAlias("/j3 /join {0} | /join {1} | /join {2}");
            AddAlias("/t /topic {0}");
            AddAlias("/say /msg {0}-");
            AddAlias("/msg /privmsg {0} :{1}-");
            AddAlias("/op /mode +o {0}");
        }
    }
}
