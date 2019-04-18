package shinningforce.imageservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.ResponseEntity;

@RestController
public class Controller {
    @Autowired
    private ImageData mImageData;

    @RequestMapping(path = "/upload", method = RequestMethod.POST)
    public ResponseEntity<String> upload(@RequestBody ImageTransmissionType image){
        mImageData.mImageQueue.add(image);
        if(mImageData.mImageQueue.size() > 5){
            mImageData.mImageQueue.poll();
        }

        return ResponseEntity.ok().body(image.mImageProgress);
    }

    @RequestMapping(path = "/download", method = RequestMethod.GET)
    public ResponseEntity<ImageTransmissionType> download(){
        ImageTransmissionType transmissionType = null;
        if(mImageData.mImageQueue.size() > 0){
            transmissionType = mImageData.mImageQueue.poll();
        }

        return ResponseEntity.ok().body(transmissionType);
    }
}
