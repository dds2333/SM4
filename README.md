# SM4
1.C#实现的国密SM4，基于WPF和BouncyCastle算法库；  
2.核心算法实现参考网上大佬的代码并调试改错，目前实现将密钥不足或超过16字节（128位，SM4算法的规定的密钥长度）  
  处理成16字节；   
3.实现的加密模式有CBC和ECB两种，其中ECB模式解密时对于不正确的密钥还没实现判断，导致解密出来的结果与明文不一致；  
4.CBC模式的初始向量iv写死在代码中为了保证关闭程序后再可以解密成功，随机生成iv也可以，只是目前还不能实现CBC模  
  式下加密完成关闭程序后再打开程序解密成功；  
5.想到解决方案，将iv写入加密文件，解密时读取出来，后续更新  
