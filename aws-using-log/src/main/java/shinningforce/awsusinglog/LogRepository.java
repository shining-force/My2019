package shinningforce.awsusinglog;

import org.springframework.data.repository.CrudRepository;

public interface LogRepository extends CrudRepository<AwsLogTable, Integer> {

}
