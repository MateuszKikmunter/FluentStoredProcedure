# Fluent Stored Procedure

Collection of classes and extension methods which allows to call stored procedures in a fluent way. 
Data in this project is mainly fetched with stored procedures just to present usage of those classes.

## Before running this project

Please restore all nugets and node packages and set web project as startup project.
Change mdf file path to your local path, so integration tests can be run without any issues.

### Sample repository method

```c#
 public async Task<Employee> GetSingleAsync(int id)
 {
    var procedure = _storedProcedureFactory.CreateStoredProcedure("GetEmployee").WithSqlParam("Id", id);
    var result = await _context.FromSqlAsync<Employee>(procedure);
    return result.FirstOrDefault();
 }
```


