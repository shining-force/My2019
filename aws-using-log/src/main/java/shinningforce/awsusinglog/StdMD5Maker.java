package shinningforce.awsusinglog;

import com.sun.istack.internal.Nullable;
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
            char[] raw = encoder.encode(mdInst.digest()).toCharArray();
            for(int iIndex = 0; iIndex < raw.length; ++ iIndex){
                if(!isCharacter(raw[iIndex])){
                    raw[iIndex] = 'X';
                }
            }
            return new String(raw);
        }catch (Exception e)
        {
            return null;
        }
    }

    public static Boolean isCharacter(char c){
        return (((c <= 'z') && (c >= 'a')) || ((c <= 'Z') && (c >= 'A')));
    }
}
