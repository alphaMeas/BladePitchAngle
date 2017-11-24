using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BewisAngleSensor
{
    public class AngleSensor
    {
        // 读取全部寄存器 返回格式,数据域有X，Y，Z三轴角度，每个数据三个字节SX XX.YY
        private byte[] CMD_READALL = new byte[] { 0x77, 0x04, 0x00, 0x04, 0x08 };

        // 设置相对零点 响应 77 05 00 85 00 8A
        private byte[] CMD_SETZERO = new byte[] { 0x77, 0x05, 0x00, 0x05, 0x01, 0x0B };

        // 设置绝对零点 响应 77 05 00 85 00 8A
        private byte[] CMD_SETABSZERO = new byte[] { 0x77, 0x05, 0x00, 0x05, 0x00, 0x0A };

        // 查询零点类型 响应 0x77, 0x05, 0x00, 0x8D, [0x00绝对零点,0xFF: 相对零点] 校验
        private byte[] CMD_GETZEROTYPE = new byte[] { 0x77, 0x04, 0x00, 0x0D, 0x11 };

        // 设置采样频率10Hz 00: 应答模式; 01 5Hz; 02 10Hz; 03:20Hz; 04:25Hz; 04:50Hz; 05:100Hz   
        private byte[] CMD_SETSAMRATE_10Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x02, 0x13 };

        // 设置采样频率5Hz 00: 应答模式; 01 5Hz; 02 10Hz; 03:20Hz; 04:25Hz; 04:50Hz; 05:100Hz   
        private byte[] CMD_SETSAMRATE_5Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x01, 0x12 };

        // 设置采样模式0Hz，即应答式
        private byte[] CMD_SETSAMRATE_0Hz = new byte[] { 0x77, 0x05, 0x00, 0x0C, 0x00, 0x11 };




    }
}
