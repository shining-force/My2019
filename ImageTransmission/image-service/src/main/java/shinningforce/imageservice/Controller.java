package shinningforce.imageservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class Controller {
    @Autowired
    private ImageData imageData;

    @RequestMapping(path = "/upload", method = RequestMethod.POST)
    public void upload(@RequestBody ImageTransmissionType image)
    {
        imageData.imageProgress = image.imageProgress;
        imageData.imageStream = image.imageStream;
    }
}
