using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
public class Encryption
{
    private static RijndaelManaged GetRijndaelManaged(String secretKey)
    {
        var keyBytes = new byte[16];
        var secretKeyBytes = Encoding.Unicode.GetBytes(secretKey);
        Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
        return new RijndaelManaged
        {
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7,
            KeySize = 128,
            BlockSize = 128,
            Key = keyBytes,
            IV = keyBytes
        };
    }
    public static byte[] Encrypt(String plainText, String secretKey)
    {
        byte[] encrypted;
        using (var cipher = GetRijndaelManaged(secretKey))
        {
            var textBytes = Encoding.Unicode.GetBytes(plainText);
            using (var encryptor = cipher.CreateEncryptor())
            {
                encrypted = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
            }
        }
        return encrypted;
    }
    public static String Decrypt(byte[] encryptedText, String secretKey)
    {
        String decrypted;
        using (var cipher = GetRijndaelManaged(secretKey))
        {
            using (var decryptor = cipher.CreateDecryptor())
            {
                var decryptedBytes = decryptor.TransformFinalBlock(encryptedText, 0, encryptedText.Length);
                decrypted = Encoding.Unicode.GetString(decryptedBytes);
            }
        }
        return decrypted;
    }
    public static void SaveToFile(string filename, string data)
    {
        File.WriteAllText(filename, data, Encoding.UTF8);
    }

    public static string LoadFromFile(string filename)
    {
        return File.ReadAllText(filename, Encoding.UTF8);
    }

    public static string GenerateKey()
    {
        using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
        {
            var randomBytes = new byte[16];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
    public static void ShowWarning()
    {
        Console.WriteLine("ПОПЕРЕДЖЕННЯ: Ця програма використовує секретний ключ для шифрування та дешифрування тексту. " +
                          "Ключ повинен бути довжиною 16 символів (128 біт) і може містити будь-які символи. " +
                          "Приклад ключа: HLJ1KoaCftNt2KQOiSecJg==");
    }

    public static string GetKey()
    {
        Console.WriteLine("Введіть '1' для введення ключа з клавіатури, '2' для генерації нового ключа, або '3' для завантаження ключа з файлу:");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                Console.WriteLine("Введіть ключ:");
                return Console.ReadLine();
            case "2":
                return GenerateKey();
            case "3":
                Console.WriteLine("Введіть ім'я файлу:");
                string filename = Console.ReadLine();
                return LoadFromFile(filename);
            default:
                Console.WriteLine("Невідома опція. Генеруємо новий ключ.");
                return GenerateKey();
        }
    }

    public static string GetText()
    {
        Console.WriteLine("Введіть '1' для введення тексту з клавіатури, або '2' для завантаження тексту з файлу:");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                Console.WriteLine("Введіть текст:");
                return Console.ReadLine();
            case "2":
                Console.WriteLine("Введіть ім'я файлу:");
                string filename = Console.ReadLine();
                return LoadFromFile(filename);
            default:
                Console.WriteLine("Невідома опція. Повертаємо пустий текст.");
                return "";
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        for (; ; )
        {
            Console.WriteLine("Виберіть режим роботи: '1' - звичайний, '2' - розшифрування");
            string mode = Console.ReadLine();

            if (mode == "2")
            {
                Console.WriteLine("Введіть '1' для введення зашифрованого тексту з клавіатури, або '2' для завантаження тексту з файлу:");
                string option = Console.ReadLine();
                string encryptedText;
                if (option == "1")
                {
                    Console.WriteLine("Введіть зашифрований текст:");
                    encryptedText = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Введіть ім'я файлу:");
                    string filename = Console.ReadLine();
                    encryptedText = Encryption.LoadFromFile(filename);
                }
                Console.WriteLine("Введіть '1' для введення ключа з клавіатури, або '2' для завантаження ключа з файлу:");
                option = Console.ReadLine();
                string key;
                if (option == "1")
                {
                    Console.WriteLine("Введіть ключ:");
                    key = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Введіть ім'я файлу:");
                    string filename = Console.ReadLine();
                    key = Encryption.LoadFromFile(filename);
                }
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                string decrypted = Encryption.Decrypt(encryptedBytes, key);

                stopwatch.Stop();
                double microseconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000000;
                Console.WriteLine("Час дешифрування: " + microseconds + " мкс");
                Console.WriteLine("Розшифрований текст: " + decrypted);
            }
            else
            {
                Encryption.ShowWarning();

                string key = Encryption.GetKey();
                Console.WriteLine("Секретний ключ: " + key);

                string input = Encryption.GetText();
                Console.WriteLine("Введений текст: " + input);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                byte[] encrypted = Encryption.Encrypt(input, key);

                stopwatch.Stop();
                double microseconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000000;
                Console.WriteLine("Час шифрування: " + microseconds + " мкс");
                Console.WriteLine("Зашифрований текст: " + Convert.ToBase64String(encrypted));

                Console.WriteLine("Чи бажаєте ви зберегти зашифрований текст? (так/ні)");
                string saveOption = Console.ReadLine();
                if (saveOption.ToLower() == "так")
                {
                    Console.WriteLine("Введіть ім'я файлу для збереження:");
                    string filename = Console.ReadLine();
                    Encryption.SaveToFile(filename, Convert.ToBase64String(encrypted));
                }
                string decrypted = Encryption.Decrypt(encrypted, key);
                Console.WriteLine("Розшифрований текст: " + decrypted);
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}


