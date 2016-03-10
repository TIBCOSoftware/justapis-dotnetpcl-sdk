using System;

namespace APGW
{
    public class CacheControlOptions
    {
        private int _expiration;
        private bool _noCache = true;

        public CacheControlOptions(int expiration) {
            _expiration = expiration;
        }

        public CacheControlOptions(bool noCache) {
            _noCache = noCache;
        }

        public bool NoCache() {
            return _noCache;
        }
    }
}

