using System.Security.Cryptography;
using System.Text;
using EchoLib.Models;
using EchoLib.Models.Crypto;
using EchoLib.Models.Misc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EchoLib.Client;

/// <summary>
/// Represents a decrypted locally stored user file.
///
/// All user files are encrypted with AES 256 using a user-provided password.
/// </summary>
public static class UserFileHelper
{
	private const int KeySize = 32; // 256 Bit
	private const int SaltSize = 16;
	private const int NonceSize = 12;
	private const int TagSize = 16;
	private const int Iterations = 100_000;

	/// <summary>
	/// Encrypts a user file to a specified location using the specified passphrase.
	/// </summary>
	/// <param name="data"></param>
	/// <param name="outputFile"></param>
	/// <param name="passphrase"></param>
	public static void Encrypt(JUserFile data, FileInfo outputFile, string passphrase)
	{
		// Serialise data
		Span<byte> pSpan = Encoding.UTF8.GetBytes(
			JsonConvert.SerializeObject(data)
		);

		// Convert span into byte array
		byte[] plaintext = pSpan.ToArray();

		// Generate required cryptographic values
		byte[] salt = RandomBytes(SaltSize);
		byte[] nonce = RandomBytes(NonceSize);
		byte[] key = DeriveKey(passphrase, salt);

		byte[] ciphertext = new byte[plaintext.Length];
		byte[] tag = new byte[TagSize];

		// Encrypt the data into ciphertext
		using AesGcm aes = new(key, TagSize);
		aes.Encrypt(nonce, plaintext, ciphertext, tag);

		// Write data to filesystem using known sizes
		using FileStream fs = new(outputFile.FullName, FileMode.Create, FileAccess.Write);
		fs.Write(salt);
		fs.Write(nonce);
		fs.Write(tag);
		fs.Write(ciphertext); // Ciphertext is written last as it is an unknown size
	}

	/// <summary>
	/// Attempts to decrypt a user file using the given passphrase.
	/// </summary>
	/// <param name="file"></param>
	/// <param name="password"></param>
	/// <param name="userFile"></param>
	/// <returns></returns>
	public static bool Decrypt(FileInfo file, string password, out JUserFile? userFile)
	{
		// Set userFile by default to null
		userFile = null;

		// Read in the file as a byte array
		byte[] data = File.ReadAllBytes(file.FullName);

		// Split up file in reverse order of what it was written
		byte[] salt = data[..SaltSize];
		byte[] nonce = data[SaltSize..(SaltSize + NonceSize)];
		byte[] tag = data[(SaltSize + NonceSize)..(SaltSize + NonceSize + TagSize)];
		byte[] ciphertext = data[(SaltSize + NonceSize + TagSize)..];

		// Plaintext byte array
		byte[] pBytes = new byte[ciphertext.Length];

		using AesGcm aes = new(DeriveKey(password, salt), TagSize);
		try
		{
			aes.Decrypt(nonce, ciphertext, tag, pBytes);
		}
		catch (CryptographicException)
		{
			return false;
		}

		// Deserialize plaintext into content
		string plaintext = Encoding.UTF8.GetString(pBytes);

		userFile = JsonConvert.DeserializeObject<JUserFile>(plaintext);
		return true;
	}

	private static byte[] DeriveKey(string passphrase, byte[] salt)
	{
		using Rfc2898DeriveBytes kdf = new(passphrase, salt, Iterations, HashAlgorithmName.SHA256);
		return kdf.GetBytes(KeySize);
	}

	private static byte[] RandomBytes(int size)
	{
		byte[] bytes = new byte[size];
		RandomNumberGenerator.Fill(bytes);
		return bytes;
	}
}