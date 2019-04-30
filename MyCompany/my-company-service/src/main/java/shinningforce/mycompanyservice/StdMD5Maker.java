package shinningforce.mycompanyservice;
import sun.misc.BASE64Encoder;
import java.security.MessageDigest;
//changes		date			version
//new			2019.04.23		V-1.00

/* changes for current project */
//changes		date
//
public class StdMD5Maker {

    public static String getMD5(String input){
        MessageDigest messageDigest;
        try{
            messageDigest = MessageDigest.getInstance("MD5");
            messageDigest.update(input.getBytes());
            BASE64Encoder encoder = new BASE64Encoder();
            return encoder.encode(messageDigest.digest());
        }catch (Exception e)
        {
            return null;
        }
    }
}
