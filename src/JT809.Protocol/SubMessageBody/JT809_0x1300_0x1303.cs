﻿using JT809.Protocol.Enums;
using JT809.Protocol.Formatters;
using JT809.Protocol.MessagePack;
using JT809.Protocol.Extensions;
using System;
using JT809.Protocol.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace JT809.Protocol.SubMessageBody
{
    /// <summary>
    /// 上传平台间消息补传请求消息
    /// <para>子业务类型标识:UP_PLATFORM_MSG_RETRAN_REQ</para>
    /// <para>描述:下级平台在接收消息时，如发现消息报文序列号不连续，则立即发送消息补传请求。上级平台收到消息补传请求后，根据请求的消息报文序列号或起始时间（当仅填写一项时。另一项全为0），重传相应的消息</para>
    /// </summary>
    public class JT809_0x1300_0x1303:JT809SubBodies, IJT809MessagePackFormatter<JT809_0x1300_0x1303>, IJT809Analyze, IJT809_2019_Version
    {
        public override ushort SubMsgId => JT809SubBusinessType.上传平台间消息补传请求消息.ToUInt16Value();

        public override string Description => "上传平台间消息补传请求消息";
        /// <summary>
        /// 对应需要重传消息地子业务类型标识
        /// </summary>
        public byte SerialCount { get; set; }

        /// <summary>
        /// 需要重传消息的起始报文序列号和结束的报文序列号。如只请求重传一个消息，则起始消息报文序列号和结束消息报文序列号相同 
        /// 8位
        /// </summary>
        public List<byte[]> SerialList { get; set; }
        /// <summary>
        /// 重传起始系统utc时间
        /// 8位
        /// </summary>
        public DateTime Time { get; set; }

        public void Analyze(ref JT809MessagePackReader reader, Utf8JsonWriter writer, IJT809Config config)
        {
            var value = new JT809_0x1300_0x1303();
            value.SerialCount = reader.ReadByte();
            writer.WriteNumber($"[{ value.SerialCount.ReadNumber()}]对应需要重传消息地子业务类型标识", value.SerialCount);
            value.SerialList = new List<byte[]>();
            writer.WriteStartArray("重传消息地子业务类型标识列表");
            for (int i = 0; i < SerialCount; i++)
            {
                var array = reader.ReadArray(8).ToArray();
                writer.WriteString($"[{array.ToHexString()}]需要重传消息的起始报文序列号和结束的报文序列号", array);
            }
            writer.WriteEndArray();
            var virtualHex = reader.ReadVirtualArray(8);
            value.Time = reader.ReadUTCDateTime();
            writer.WriteString($"[{virtualHex.ToArray().ToHexString()}]重传起始系统utc时间", value.Time);
        }

        public JT809_0x1300_0x1303 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
        {
            var value= new JT809_0x1300_0x1303();
            value.SerialCount = reader.ReadByte();
            value.SerialList = new List<byte[]>();
            for (int i = 0; i < SerialCount; i++)
            {
                value.SerialList.Add(reader.ReadArray(8).ToArray());
            }
            value.Time = reader.ReadUTCDateTime();
            return value;
        }

        public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1300_0x1303 value, IJT809Config config)
        {
            writer.WriteByte(value.SerialCount);
            foreach (var item in value.SerialList)
            {
                writer.WriteArray(item);
            }          
            writer.WriteUTCDateTime(value.Time);
        }
    }
}
