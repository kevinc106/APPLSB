#if (UNITY_EDITOR)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace CreationStates
{
    class EventStreamParserAdapter : IParser
    {
        private readonly IEnumerator<ParsingEvent> enumerator;

        public EventStreamParserAdapter(IEnumerable<ParsingEvent> events)
        {
            enumerator = events.GetEnumerator();
        }

        public ParsingEvent Current
        {
            get
            {
                return enumerator.Current;
            }
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }
    }
}
#endif