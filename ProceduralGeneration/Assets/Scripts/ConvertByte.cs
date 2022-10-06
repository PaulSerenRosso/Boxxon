using System;
using UnityEngine;

public class ConvertByte : MonoBehaviour
{
    private void Start()
    {
        uint[] bits = new uint[]
            {0b_1100_1110,0b_1110_1110,0b_0100_1010 };
        ConvertBytesToDecimal(bits);
    }
    public void ConvertBytesToDecimal(uint[] bits)
    {
        uint[] bitsWithoutMajorBit = new uint[3];
        bool[] signedByte = new bool[3];
        for (int i = 0; i < bits.Length; i++)
        {
            if ((bits[i] >> 7) != 0)
            {
                signedByte[i] = true;
            }
            else
            {
                signedByte[i] = false;
            }
            bitsWithoutMajorBit[i] = bits[i] & 0b_0111_1111;
        }

        uint finalBit = 0;
        finalBit = bitsWithoutMajorBit[0];
        for (int i = 0; i < bitsWithoutMajorBit.Length-1; i++)
        {
            finalBit = finalBit << 7;
            finalBit = bitsWithoutMajorBit[i] | finalBit;
        }
            Debug.Log(Convert.ToString(finalBit, 2));
            Debug.Log(finalBit);
            for (int i = 0; i < signedByte.Length; i++)
            {
                Debug.Log(signedByte[i]+"  "+i);
            }
    }
    
}
