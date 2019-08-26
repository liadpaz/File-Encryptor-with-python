from pyAesCrypt import encryptFile, decryptFile
import sys
import os
import time

bufferSize = 64 * 1024

path = sys.argv[1]
action = sys.argv[2]
password = sys.argv[3]

try:
    # encryption
    if action in "encrypt":
        encryptFile(path, f"{path}.aes", password, bufferSize)
        os.remove(path)
        print("Successfully encrypted your file!")

    # decrypt
    elif action in "decrypt":
        decryptFile(path, path[:-4], password, bufferSize)
        os.remove(path)
        print("Successfully decrypted your file!")

    else:
        print(action + "\nEnter a valid action!")

except Exception as error:
    print(error)

finally:
    time.sleep(1)
