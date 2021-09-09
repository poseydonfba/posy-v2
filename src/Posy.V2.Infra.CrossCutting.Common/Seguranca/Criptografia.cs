using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Posy.V2.Infra.CrossCutting.Common.Seguranca
{
    public class Criptografia
    {
        /// <summary>     
        /// Vetor de bytes utilizados para a criptografia (Chave Externa)   
        /// O vetor de inicialização deve possuir 16 caracteres.
        /// </summary>     
        public static string vetorIni = "";

        /// <summary>     
        /// Representação de valor em base 64 (Chave Interna)    
        /// O Valor representa a transformação para base64 de     
        /// um conjunto de 32 caracteres (8 * 32 = 256bits)   
        /// </summary>     
        public const string cryptoKey = "";

        #region METODOS PUBLICO

        public static string Encrypt(string texto)
        {
            return BaseEncrypt(cryptoKey, vetorIni, texto);
        }

        public static string Decrypt(string texto)
        {
            return BaseDecrypt(cryptoKey, vetorIni, texto);
        }

        public static string Encrypt(string chave, string vetorInicializacao, string texto)
        {
            return BaseEncrypt(chave, vetorInicializacao, texto);
        }

        public static string Decrypt(string chave, string vetorInicializacao, string texto)
        {
            return BaseDecrypt(chave, vetorInicializacao, texto);
        }

        public static string EncryptSenha(string password)
        {
            password += "|2d331cca-f6c0-40c0-bb43-6e32989c2881";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(password));
            System.Text.StringBuilder sbString = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sbString.Append(data[i].ToString("x2"));
            return sbString.ToString();
        }

        #endregion

        /// <summary>     
        /// Metodo de criptografia de valor     
        /// </summary>     
        /// <param name="text">valor a ser criptografado</param>     
        /// <returns>valor criptografado</returns>
        private static string BaseEncrypt(string chave, string vetorInicializacao, string texto)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia
                if (!string.IsNullOrEmpty(texto))
                {
                    // Cria instancias de vetores de bytes com as chaves                
                    byte[] bKey = Convert.FromBase64String(chave);
                    byte[] bIV = Convert.FromBase64String(vetorInicializacao);
                    byte[] bText = new UTF8Encoding().GetBytes(texto);

                    // Instancia a classe de criptografia Rijndael
                    using (Rijndael rijndael = new RijndaelManaged())
                    {
                        // Define o tamanho da chave "256 = 8 * 32"                
                        // Lembre-se: chaves possíves:                
                        // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                        rijndael.KeySize = 256;

                        // Cria o espaço de memória para guardar o valor criptografado:                
                        using (MemoryStream mStream = new MemoryStream())
                        {
                            // Instancia o encriptador                 
                            using (CryptoStream encryptor = new CryptoStream(mStream, rijndael.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                            {
                                // Faz a escrita dos dados criptografados no espaço de memória
                                encryptor.Write(bText, 0, bText.Length);
                                // Despeja toda a memória.                
                                encryptor.FlushFinalBlock();
                                // Pega o vetor de bytes da memória e gera a string criptografada                
                                return Convert.ToBase64String(mStream.ToArray());
                            }
                        }
                    }
                }
                else
                {
                    // Se a string for vazia retorna nulo                
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção            
                throw new ApplicationException("Erro ao criptografar", ex);
            }
        }

        /// <summary>     
        /// Pega um valor previamente criptografado e retorna o valor inicial 
        /// </summary>     
        /// <param name="text">texto criptografado</param>     
        /// <returns>valor descriptografado</returns>     
        private static string BaseDecrypt(string chave, string vetorInicializacao, string texto)
        {
            try
            {
                // Se a string não está vazia, executa a criptografia           
                if (!string.IsNullOrEmpty(texto))
                {
                    // Cria instancias de vetores de bytes com as chaves                
                    byte[] bKey = Convert.FromBase64String(chave);
                    byte[] bIV = Convert.FromBase64String(vetorInicializacao);
                    byte[] bText = Convert.FromBase64String(texto);

                    // Instancia a classe de criptografia Rijndael                
                    using (Rijndael rijndael = new RijndaelManaged())
                    {
                        // Define o tamanho da chave "256 = 8 * 32"                
                        // Lembre-se: chaves possíves:                
                        // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                        rijndael.KeySize = 256;

                        // Cria o espaço de memória para guardar o valor DEScriptografado:               
                        using (MemoryStream mStream = new MemoryStream())
                        {
                            // Instancia o Decriptador                 
                            using (CryptoStream decryptor = new CryptoStream(mStream, rijndael.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                            {
                                // Faz a escrita dos dados criptografados no espaço de memória   
                                decryptor.Write(bText, 0, bText.Length);
                                // Despeja toda a memória.                
                                decryptor.FlushFinalBlock();
                                // Instancia a classe de codificação para que a string venha de forma correta         
                                UTF8Encoding utf8 = new UTF8Encoding();
                                // Com o vetor de bytes da memória, gera a string descritografada em UTF8       
                                return utf8.GetString(mStream.ToArray());
                            }
                        }
                    }
                }
                else
                {
                    // Se a string for vazia retorna nulo                
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Se algum erro ocorrer, dispara a exceção            
                throw new ApplicationException("Erro ao descriptografar", ex);
            }
        }
    }
}
