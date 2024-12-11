Overview

This is a console application written in C# for encrypting and decrypting text using the Rijndael (AES) encryption algorithm. It also includes features for key generation, saving/loading data to/from files, and measuring performance.

Features

Encrypt and Decrypt: Easily encrypt and decrypt text using a secret key.

Key Management:

Generate a new random secret key.

Input a key manually.

Load a key from a file.

Data Management:

Save encrypted data to a file.

Load data from a file.

Performance Monitoring: Measure encryption and decryption times.

Technologies Used

Language: C#

Encryption Framework: System.Security.Cryptography

Utilities:

System.Text for text encoding.

System.Diagnostics for performance monitoring.

System.IO for file operations.

How to Use

Prerequisites

.NET runtime or SDK installed on your system.

Basic knowledge of working with console applications.

User Guide

Encryption Mode

Choose the encryption mode by selecting option 1.

Provide the input text:

Type it manually.

Load it from a file.

Provide or generate a secret key:

Input it manually.

Generate a new key.

Load it from a file.

View the encrypted text and optionally save it to a file.

Observe the encryption time displayed on the console.

Decryption Mode

Choose the decryption mode by selecting option 2.

Provide the encrypted text:

Type it manually.

Load it from a file.

Provide the corresponding secret key:

Input it manually.

Load it from a file.

View the decrypted text and observe the decryption time.