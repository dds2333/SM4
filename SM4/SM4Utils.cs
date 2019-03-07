using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities.Encoders;

namespace SM4
{
    class SM4Utils
    {
        public string secretKey = "";
        public string iv = "";
        public bool hexString = false;
        public byte[] secretKeyBuff;


        public string Encrypt_ECB(string plainData)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4_calss.SM4_ENCRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.ASCII.GetBytes(secretKey);
            }

            SM4_calss sm4 = new SM4_calss();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.Default.GetBytes(plainData));
            //return encrypted;
            string cipherText = Encoding.Default.GetString(Hex.Encode(encrypted));
            return cipherText;
        }

        public string Decrypt_ECB(string cipherText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4_calss.SM4_DECRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.ASCII.GetBytes(secretKey);
            }

            SM4_calss sm4 = new SM4_calss();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
            if (decrypted == null)
            {
                return string.Empty;
            }
            else
            {
                return Encoding.Default.GetString(decrypted);
            }
        }          

        public string Encrypt_CBC(string plainData)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4_calss.SM4_ENCRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }

            SM4_calss sm4 = new SM4_calss();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Encoding.Default.GetBytes(plainData));

            //return Hex.Encode(encrypted);
            //return encrypted;
            string cipherText = Encoding.Default.GetString(Hex.Encode(encrypted));
            return cipherText;
        }

        public string Decrypt_CBC(string cipherData)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4_calss.SM4_DECRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }

            SM4_calss sm4 = new SM4_calss();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherData));
            if (decrypted == null)
            {
                return string.Empty;
            }
            else
            {
                return Encoding.Default.GetString(decrypted);
            }            
            //return decrypted;
        }

        //public string Encrypt_ECB(string plainText)
        //{
        //    SM4_Context ctx = new SM4_Context();
        //    ctx.isPadding = true;
        //    ctx.mode = SM4.SM4_ENCRYPT;

        //    byte[] keyBytes;
        //    if (hexString)
        //    {
        //        keyBytes = Hex.Decode(secretKey);
        //    }
        //    else
        //    {
        //        keyBytes = Encoding.ASCII.GetBytes(secretKey);
        //    }

        //    SM4 sm4 = new SM4();
        //    sm4.sm4_setkey_enc(ctx, keyBytes);
        //    byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.ASCII.GetBytes(plainText));

        //    string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        //    return cipherText;
        //}

        //public byte[] Encrypt_ECB(byte[] plainBytes, byte[] keyBytes)
        //{
        //    SM4_Context ctx = new SM4_Context();
        //    ctx.isPadding = false;
        //    ctx.mode = SM4.SM4_ENCRYPT;

        //    SM4 sm4 = new SM4();
        //    sm4.sm4_setkey_enc(ctx, keyBytes);
        //    byte[] encrypted = sm4.sm4_crypt_ecb(ctx, plainBytes);
        //    return encrypted;

        //    //return Hex.Encode(encrypted);
        //}

        //public string Decrypt_ECB(string cipherText)
        //{
        //    SM4_Context ctx = new SM4_Context();
        //    ctx.isPadding = true;
        //    ctx.mode = SM4.SM4_DECRYPT;

        //    byte[] keyBytes;
        //    if (hexString)
        //    {
        //        keyBytes = Hex.Decode(secretKey);
        //    }
        //    else
        //    {
        //        keyBytes = Encoding.ASCII.GetBytes(secretKey);
        //    }

        //    SM4 sm4 = new SM4();
        //    sm4.sm4_setkey_dec(ctx, keyBytes);
        //    byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
        //    return Encoding.ASCII.GetString(decrypted);
        //}
        //public string Encrypt_CBC(string plainText)
        //{
        //    SM4_Context ctx = new SM4_Context();
        //    ctx.isPadding = true;
        //    ctx.mode = SM4.SM4_ENCRYPT;

        //    byte[] keyBytes;
        //    byte[] ivBytes;
        //    if (hexString)
        //    {
        //        keyBytes = Hex.Decode(secretKey);
        //        ivBytes = Hex.Decode(iv);
        //    }
        //    else
        //    {
        //        keyBytes = Encoding.ASCII.GetBytes(secretKey);
        //        ivBytes = Encoding.ASCII.GetBytes(iv);
        //    }

        //    SM4 sm4 = new SM4();
        //    sm4.sm4_setkey_enc(ctx, keyBytes);
        //    byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Encoding.ASCII.GetBytes(plainText));

        //    string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        //    return cipherText;
        //}

        //public string Decrypt_CBC(string cipherText)
        //{
        //    SM4_Context ctx = new SM4_Context();
        //    ctx.isPadding = true;
        //    ctx.mode = SM4.SM4_DECRYPT;

        //    byte[] keyBytes;
        //    byte[] ivBytes;
        //    if (hexString)
        //    {
        //        keyBytes = Hex.Decode(secretKey);
        //        ivBytes = Hex.Decode(iv);
        //    }
        //    else
        //    {
        //        keyBytes = Encoding.ASCII.GetBytes(secretKey);
        //        ivBytes = Encoding.ASCII.GetBytes(iv);
        //    }

        //    SM4 sm4 = new SM4();
        //    sm4.sm4_setkey_dec(ctx, keyBytes);
        //    byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
        //    return Encoding.ASCII.GetString(decrypted);
        //}
    }
}
