package shinningforce.istodayfriday;

import org.apache.tomcat.util.http.fileupload.ByteArrayOutputStream;
import org.apache.tomcat.util.http.fileupload.IOUtils;
import org.springframework.lang.Nullable;

import javax.imageio.ImageIO;
import javax.imageio.stream.ImageOutputStream;
import javax.servlet.http.HttpServletResponse;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.util.Random;

public class DayPicCreater {
    private BufferedImage mImage;

    public DayPicCreater(Integer dayOfWeek, Integer width, Integer height){
        mImage = new BufferedImage(width, height, BufferedImage.TYPE_INT_RGB);
        Graphics graphics = mImage.getGraphics();

        int lower = 40 * (dayOfWeek - 1);
        int upper = lower + 40;
        graphics.setColor(getRandColor(lower, upper));
        graphics.fillRect(0, 0, width, height);
        graphics.dispose();
    }
    @Nullable
    public Throwable ImageOutput(HttpServletResponse response){
        response.setContentType("image/jpeg;charset=UTF-8");
        ByteArrayOutputStream bs = new ByteArrayOutputStream();
        ImageOutputStream imOut;
        try{
            imOut = ImageIO.createImageOutputStream(bs);
            ImageIO.write(mImage,"jpg",imOut);
            InputStream is = new ByteArrayInputStream(bs.toByteArray());
            IOUtils.copy(is, response.getOutputStream());
            return null;
        }catch (Exception e){
            return e;
        }
    }

    private Color getRandColor(int lower, int upper) {
        Random random = new Random();
        if (lower > 255)
            lower = 255;
        if (upper > 255)
            upper = 255;
        int r = lower + random.nextInt(upper - lower);
        int g = lower + random.nextInt(upper - lower);
        int b = lower + random.nextInt(upper - lower);
        return new Color(r, g, b);
    }
}
