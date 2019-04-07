package shinningforce.imageservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.ResponseEntity;

@RestController
public class Controller {
    @Autowired
    private ImageData imageData;

    @RequestMapping(path = "/upload", method = RequestMethod.POST)
    public ResponseEntity<String> upload(@RequestBody ImageTransmissionType image)
    {
        imageData.imageProgress = image.imageProgress;
        imageData.imageStream = image.imageStream;

        return ResponseEntity.ok().body(null);
    }

    @RequestMapping(path = "/download", method = RequestMethod.GET)
    public ResponseEntity<ImageTransmissionType> download()
    {
        ImageTransmissionType transmissionType = new ImageTransmissionType();
        transmissionType.imageProgress = imageData.imageProgress;
        transmissionType.imageStream = imageData.imageStream;
        return ResponseEntity.ok().body(transmissionType);
    }
}
