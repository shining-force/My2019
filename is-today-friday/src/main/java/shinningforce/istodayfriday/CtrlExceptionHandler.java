package shinningforce.istodayfriday;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.servlet.mvc.method.annotation.ResponseEntityExceptionHandler;

import javax.servlet.http.HttpServletRequest;

@ControllerAdvice(basePackageClasses = ServiceController.class)
public class CtrlExceptionHandler extends ResponseEntityExceptionHandler {
    @ExceptionHandler(Exception.class)
    ResponseEntity<RequestOutCome> handleControllerException(HttpServletRequest request, Throwable ex) {
        RequestOutCome requestOutCome = new RequestOutCome();
        requestOutCome.setSuccess(false);
        requestOutCome.setmMsg("Before controller:" + ex.getLocalizedMessage());
        return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(requestOutCome);
    }
}
