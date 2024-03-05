using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Bouncer.Saves
{
    public class JsonSaveManager : ISaveManager
    {
        //filename pDB
        private const string KEY = "SdsSAhL2NgqKIqIkNkxBjxUAU4I0xp7WHR2w9tBCWaQ=";
        private const string IV = "2x9BCdnWI9dOaAmqjWjyoA==";

        private void WriteEncryptedData<T>(T data, FileStream stream)
        {
            using Aes aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(KEY);
            aesProvider.IV = Convert.FromBase64String(IV);

            using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
            using CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
        }

        private T ReadEncryptedData<T>(string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);

            using Aes aesProvider = Aes.Create();
            //Debug.Log("KEY: "+ Convert.ToBase64String(aesProvider.Key));
            //Debug.Log("IV: " + Convert.ToBase64String(aesProvider.IV));
            aesProvider.Key = Convert.FromBase64String(KEY);
            aesProvider.IV = Convert.FromBase64String(IV);

            using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);
            using MemoryStream decryptionStream = new MemoryStream(fileBytes);
            using CryptoStream cryptoStream = new CryptoStream(decryptionStream, cryptoTransform, CryptoStreamMode.Read);
            using StreamReader reader = new StreamReader(cryptoStream);

            string result = reader.ReadToEnd();
            //Debug.Log($"Decryption: {result}");
            return JsonConvert.DeserializeObject<T>(result);
        }

        public bool SaveData<T>(string relativePath, T data, bool encrypted)
        {
            string path = Application.persistentDataPath + relativePath;

            try
            {
                if (File.Exists(path))
                {
                    //Debug.Log("File exists! Recreating.");
                    File.Delete(path);
                    //return true;
                }
                else
                {
                    //Debug.Log("Creating file for the first time!");
                }

                using FileStream stream = File.Create(path);
                if (encrypted)
                {
                    WriteEncryptedData(data, stream);
                }
                else
                {
                    stream.Close();
                    File.WriteAllText(path, JsonConvert.SerializeObject(data));
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while saving data: {e.Message} {e.StackTrace}");
                return false;
            }
        }

        public T LoadData<T>(string relativePath, bool encrypted)
        {
            string path = Application.persistentDataPath + relativePath;

            if (!File.Exists(path))
            {
                Debug.LogError("No file found!");
                throw new FileNotFoundException();
            }

            try
            {
                T data;
                if (encrypted)
                {
                    data = ReadEncryptedData<T>(path);
                }
                else
                {
                    data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                }
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
