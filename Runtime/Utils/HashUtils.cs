namespace EgorLin.Keys.Utils
{
	public static class HashUtils
    {
        private const uint FnvPrime32 = 16_777_619;
        private const ulong FnvPrime64 = 1_099_511_628_211;
        
        private const uint FnvOffset32 = 2_166_136_261;
        private const ulong FnvOffset64 = 14_695_981_039_346_656_037;

        /// <summary>
        /// FNV-1a hash
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int StringToHash32(string text)
        {
            unchecked
            {
                uint hash = FnvOffset32;

                for (int i = 0; i < text.Length; i++)
                {
                    hash ^= text[i];
                    hash *= FnvPrime32;
                }

                return (int)hash;
            }
        }
        
        /// <summary>
        /// FNV-1a 64-bit hash
        /// </summary>
        public static long StringToHash64(string text)
        {
            unchecked
            {
                ulong hash = FnvOffset64;

                for (int i = 0; i < text.Length; i++)
                {
                    hash ^= text[i];
                    hash *= FnvPrime64;
                }

                return (long)hash;
            }
        }
	}
}