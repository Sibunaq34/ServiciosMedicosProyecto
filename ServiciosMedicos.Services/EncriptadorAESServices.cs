using System;
using System.Security.Cryptography;
using System.Text;

namespace Servicios_Medicos.Services
{
    public class EncriptadorAESServices 
    {
        private readonly byte[] clave =
            Encoding.UTF8.GetBytes("71962840184936251827825463019826");

        public string Encriptar(string textoPlano)
        {
            byte[] nonce = new byte[12];
            RandomNumberGenerator.Fill(nonce);

            byte[] textoBytes = Encoding.UTF8.GetBytes(textoPlano);
            byte[] textoCifrado = new byte[textoBytes.Length];
            byte[] tag = new byte[16];

            using (var aes = new AesGcm(clave))
            {
                aes.Encrypt(
                    nonce,
                    textoBytes,
                    textoCifrado,
                    tag);
            }

            byte[] resultado = new byte[
                nonce.Length +
                tag.Length +
                textoCifrado.Length];

            Buffer.BlockCopy(
                nonce,
                0,
                resultado,
                0,
                nonce.Length);

            Buffer.BlockCopy(
                tag,
                0,
                resultado,
                nonce.Length,
                tag.Length);

            Buffer.BlockCopy(
                textoCifrado,
                0,
                resultado,
                nonce.Length + tag.Length,
                textoCifrado.Length);

            return Convert.ToBase64String(resultado);
        }

        public string Desencriptar(string textoCifradoBase64)
        {
            try { 
                byte[] datos =
                    Convert.FromBase64String(textoCifradoBase64);

                byte[] nonce = new byte[12];
                byte[] tag = new byte[16];
                byte[] textoCifrado =
                    new byte[datos.Length - 28];

                Buffer.BlockCopy(datos, 0, nonce, 0, 12);

                Buffer.BlockCopy(datos, 12, tag, 0, 16);

                Buffer.BlockCopy(datos, 28, textoCifrado, 0, textoCifrado.Length);

                byte[] textoPlano =
                    new byte[textoCifrado.Length];

                using (var aes = new AesGcm(clave))
                {
                    aes.Decrypt(
                        nonce,
                        textoCifrado,
                        tag,
                        textoPlano);
                }

                return Encoding.UTF8.GetString(textoPlano);
            } catch {
                throw new ArgumentException("La contraseña no es válida");
            }
        }

        public bool CompararPassword(
            string passwordIngresada,
            string passwordCifradaBD) //passwordCifradaBD  remplazar
        {
           string passwordBD = Desencriptar(passwordCifradaBD);
      
            return passwordIngresada == passwordBD;
        }
    }
}