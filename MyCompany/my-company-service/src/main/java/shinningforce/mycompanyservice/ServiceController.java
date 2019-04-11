package shinningforce.mycompanyservice;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class ServiceController {

    @RequestMapping(method = RequestMethod.GET, path = "/bg/techInfo")
    ResponseEntity<String> OnGetTechInfo(@RequestParam(name = "product",defaultValue = "")String product){
        return ResponseEntity.ok().body("bg/techInfo");
    }

    @RequestMapping(method = RequestMethod.GET, path = "/bg/movie")
    ResponseEntity<String> OnGetMovie(@RequestParam(name = "product",defaultValue = "")String product){
        return ResponseEntity.ok().body("bg/movie");
    }

    @RequestMapping(method = RequestMethod.GET, path = "/bg/bookList")
    ResponseEntity<String> OnGetBookList(@RequestParam(name = "product",defaultValue = "")String product){
        return ResponseEntity.ok().body("bg/bookList");
    }

    @RequestMapping(method = RequestMethod.GET, path = "/bg/softList")
    ResponseEntity<String> OnGetSoftList(@RequestParam(name = "product",defaultValue = "")String product){
        return ResponseEntity.ok().body("bg/softList");
    }

    @RequestMapping(method = RequestMethod.GET, path = "/bg/firmList")
    ResponseEntity<String> OnGetFirmList(@RequestParam(name = "product",defaultValue = "")String product){
        return ResponseEntity.ok().body("bg/firmList");
    }

    @RequestMapping(method = RequestMethod.GET, path = "/bg/book")
    ResponseEntity<String> OnGetBook(@RequestParam(name = "product",defaultValue = "")String product){
        return ResponseEntity.ok().body("bg/book");
    }

    @RequestMapping(method = RequestMethod.GET, path = "/bg/soft")
    ResponseEntity<String> OnGetSoft(@RequestParam(name = "product",defaultValue = "")String product){
        return ResponseEntity.ok().body("bg/soft");
    }

    @RequestMapping(method = RequestMethod.GET, path = "/bg/firm")
    ResponseEntity<String> OnGetFirm(@RequestParam(name = "product",defaultValue = "")String product){
        return ResponseEntity.ok().body("bg/firm");
    }
}
