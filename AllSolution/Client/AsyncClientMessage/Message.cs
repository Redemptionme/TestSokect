﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncClientMessage
{
    class Message
    {
        private byte[] data = new byte[1024];
        private int startIndex = 0;//我们存取了多少个字节的数据在数组里面
        public void AddCount(int count) {
            startIndex += count;
        }
        public byte[] Data {
            get { return data; }
        }
        public int StartIndex {
            get {
                return startIndex;
            }
        }
        public int RemindSize {
            get {
                return data.Length - startIndex;
            }
        }
        //解析数据
        public void ReadMessage() {
            while (true) {
                if (startIndex <= 4) {
                    return;
                }
                int count = BitConverter.ToInt32(data, 0);
                if ((startIndex - 4) >= count) {
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出来一条数据：" + s);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);
                }
                else {
                    break;
                }
            }

        }
        /// <summary>
        /// 得到数据的约定形式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string data) {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataLength = dataBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(dataLength);
            byte[] Bytes = lengthBytes.Concat(dataBytes).ToArray();
            return Bytes;
        }


    }
}