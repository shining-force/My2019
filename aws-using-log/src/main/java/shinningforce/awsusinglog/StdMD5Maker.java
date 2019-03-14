package shinningforce.awsusinglog;

import org.springframework.lang.Nullable;
import sun.misc.BASE64Encoder;

import java.security.MessageDigest;

public class StdMD5Maker {
    @Nullable
    public static String getMD5(String input){
        MessageDigest mdInst;
        try{
            mdInst = MessageDigest.getInstance("MD5");
            mdInst.update(input.getBytes());
            BASE64Encoder encoder = new BASE64Encoder();
            return encoder.encode(mdInst.digest());
        }catch (Exception e)
        {
            return null;
        }
    }
}
