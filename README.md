# SM4
1.C#实现的国密SM4，基于WPF和BouncyCastle算法库；  
2.核心算法实现参考网上大佬的代码并调试改错，目前实现将密钥不足或超过16字节（128位，SM4算法的规定的密钥长度）处理成16字节；   
3.实现的加密模式有CBC和ECB两种，其中ECB模式解密时对于不正确的密钥还没实现判断，导致解密出来的结果与明文不一致；  
4.CBC模式的初始向量iv写死在代码中为了保证关闭程序后再可以解密成功，随机生成iv也可以，只是目前还不能实现CBC模  
  式下加密完成关闭程序后再打开程序解密成功；  
5.想到解决方案，将iv写入加密文件，解密时读取出来，后续更新  
  
更新：  
1.已解决两种模式解密时密钥不正确的判断；  
2.CBC模式iv的问题已解决，通过加密时同密文一起写入保存到密文文件，解密时读取出来，从而保证密文随机以及加密之后关闭程序可以再解密；  
3.程序只是简单实现了加解密，有需要的可以参考进一步开发

注：  
ECB模式的代码以及注释有差错，将EBC改为ECB  
Bouncycastle已经实现了SM2、SM3、SM4算法，实际运用参考BC库吧  

C#中调用Bouncycastle的SM4加密算法：  
拿CBC模式来说，需要提供key、iv、明文，对于key，可以使用SM3算法对其进行哈希截取一半的值作为实际运算中的密钥（可再使用伪随机类SecureRandom生成随机数拼接输入的key，增加随机性），iv也可以使用SecureRandom来生成，不过为了能解密加密后的文件，应把iv连同密文一起写入文件。  

代码示例：  
SM3:  
  string key_str="wohaicai"  
  byte[] key_tmp=Encoding.Default.GetBytes(key_str);  
  byte[] digest;  
  org.bouncycastle.crypto.Digest md = new org.bouncycastle.crypto.digests.SM3Digest();  
  md.update(key_tmp,0,key_tmp.length);  
  digest = new byte[md.getDigestSize()];  
  md.doFinal(digest, 0);  
SM4:  
  //加密  
  byte[] plaintext = Encoding.ASCII.GetBytes("caiji");  
  byte[] keyBytes = Encoding.ASCII.GetBytes("0123456789ABCDEF");  
  byte[] iv = Encoding.ASCII.GetBytes("0123456789ABCDEF");  
  KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);  
  ParametersWithIV keyParamWithIv = new ParametersWithIV(key, iv);  
  IBufferedCipher inCipher = CipherUtilities.GetCipher("SM4/CBC/PKCS7Padding");  
  inCipher.Init(true, keyParamWithIv);  
  byte[] cipher = inCipher.DoFinal(plaintext);  
  
  //解密  
  byte[] cipher=...;//比如从文件读取  
  byte[] keyBytes = Encoding.ASCII.GetBytes("0123456789ABCDEF");  
  byte[] iv = Encoding.ASCII.GetBytes("0123456789ABCDEF");  
  KeyParameter key = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);  
  ParametersWithIV keyParamWithIv = new ParametersWithIV(key, iv);  
  IBufferedCipher inCipher = CipherUtilities.GetCipher("SM4/CBC/PKCS7Padding");  
  inCipher.Init(false, keyParamWithIv);  
  byte[] plain = inCipher.DoFinal(cipher);  
