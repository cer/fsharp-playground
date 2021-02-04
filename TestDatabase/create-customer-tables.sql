
CREATE TABLE $(TRAM_SCHEMA).dbo._Customers (
  id INT IDENTITY(1,1),
  name VARCHAR(450)
);

Go

SELECT * FROM $(TRAM_SCHEMA).dbo._Customers;

Go
