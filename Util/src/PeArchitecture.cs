using System;

namespace hoTools.Utils
{
    public class PeArchitecture
    {
        public enum Architecure
        {
            Error = 0,
            X86 = 0x10B,
            X64 = 0x20B,
            None = 1
        }

        /// <summary>
        /// Get Architecture of an exe/dll
        ///
        /// 0x10B - PE32  format: x86, 32 Bit
        /// 0x20B - PE32+ format: x64 or Any CPU, 32 or 64 Bit
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static Architecure GetPeArchitecture(string pFilePath)
        {
            try
            {
                using (System.IO.FileStream fStream = new System.IO.FileStream(pFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (System.IO.BinaryReader bReader = new System.IO.BinaryReader(fStream))
                    {
                        // Check the MZ signature
                        if (bReader.ReadUInt16() == 23117)
                        {
                            // Seek to e_lfanew.
                            fStream.Seek(0x3A, System.IO.SeekOrigin.Current);

                            // Seek to the start of the NT header.
                            fStream.Seek(bReader.ReadUInt32(), System.IO.SeekOrigin.Begin);

                            if (bReader.ReadUInt32() == 17744) // Check the PE\0\0 signature.
                            {
                                // Seek past the file header,
                                fStream.Seek(20, System.IO.SeekOrigin.Current);

                                // Read the magic number of the optional header.
                                var arch = bReader.ReadUInt16();
                                if (arch == Architecure.X64.GetHashCode())
                                    return Architecure.X64;
                                if (arch == Architecure.X86.GetHashCode())
                                    return Architecure.X86;

                                return Architecure.None;


                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Architecure.Error;
            }

            // If architecture returns 0, there has been an error.
            return Architecure.None;
        }
    }
}
