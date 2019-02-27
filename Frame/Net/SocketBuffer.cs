using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallbackRecvOver(byte[] allData);

public class SocketBuffer {

    private byte[] headByte;
    private byte[] allRecvData;//接收到的数据
    private byte headLength = 6;
    private int curRecvLength;//当前接收到的数据长度
    private int allDataLength;//接收到的数据总长度

    public SocketBuffer(byte headLength, CallbackRecvOver tmpOver) {
        this.headLength = headLength;
        this.headByte = new byte[this.headLength];
        this.callbackRecvOver = tmpOver;
    }

    public void RecvByte(byte[] recvByte, int realLength) {
        if (realLength == 0)
            return;

        //当前接收到的数据长度 < 数据头的长度
        if (curRecvLength < headByte.Length)
            RecvHead(recvByte, realLength);
        else
        {
            //接收的总长度
            int tmpLength = curRecvLength + realLength;
            if (tmpLength == allDataLength)
                RecvOneAll(recvByte, realLength);
            else if (tmpLength > allDataLength)
                RecvLarger(recvByte, realLength);
            else
                RecvSmall(recvByte, realLength);
        }
    }

    private void RecvHead(byte[] recvByte, int realLength) {
        //差多少字节可以组成一个数据头
        int tmpReal = headByte.Length - curRecvLength;
        int tmpLength = curRecvLength + realLength;

        if (tmpLength < headByte.Length)
        {
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, realLength);
            curRecvLength += realLength;
        }
        else
        {
            Buffer.BlockCopy(recvByte, 0, headByte, curRecvLength, tmpReal);
            curRecvLength += tmpReal;

            //取出4字节转换int
            allDataLength = BitConverter.ToInt32(headByte, 0) + headLength;
            allRecvData = new byte[allDataLength];//head + body

            //allRecvData已经包含数据头
            Buffer.BlockCopy(headByte, 0, allRecvData, 0, headLength);

            int tmpRemin = realLength - tmpReal;

            //表示recByte是否还有数据
            if (tmpRemin > 0)
            {
                byte[] tmpByte = new byte[tmpRemin];
                Buffer.BlockCopy(recvByte, tmpReal, tmpByte, 0, tmpRemin);
                RecvByte(tmpByte, tmpRemin);
            }
            else
                RecvOneMsgOver();//只有消息头的情况
        }
    }

    private void RecvOneAll(byte[] recvByte, int realLength) {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);
        curRecvLength += realLength;
        RecvOneMsgOver();
    }

    private void RecvLarger(byte[] recvByte, int realLength) {
        int tmpLength = allDataLength - curRecvLength;
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, tmpLength);
        curRecvLength += tmpLength;
        RecvOneMsgOver();

        int reminLength = realLength - tmpLength;
        byte[] reminByte = new byte[reminLength];
        Buffer.BlockCopy(recvByte, tmpLength, reminByte, 0, reminLength);

        RecvByte(reminByte, reminLength);
    }

    private void RecvSmall(byte[] recvByte, int realLength) {
        Buffer.BlockCopy(recvByte, 0, allRecvData, curRecvLength, realLength);
        curRecvLength += realLength;
    }

    #region 数据接收完成回调
    
    CallbackRecvOver callbackRecvOver;

    private void RecvOneMsgOver() {
        if (callbackRecvOver != null)
            callbackRecvOver(allRecvData);
        curRecvLength = 0;
        allDataLength = 0;
        allRecvData = null;
    }

    #endregion
}
