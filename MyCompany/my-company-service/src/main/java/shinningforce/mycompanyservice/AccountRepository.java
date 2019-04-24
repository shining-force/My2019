package shinningforce.mycompanyservice;

import org.springframework.data.repository.CrudRepository;

public interface AccountRepository extends CrudRepository<DBAccountTable, Integer> {
}

