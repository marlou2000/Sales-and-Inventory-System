CREATE TABLE items(
	item_id INT PRIMARY KEY IDENTITY(1,1),
	item_name VARCHAR(255) NOT NULL,
	item_price FLOAT NOT NULL,
	item_description TEXT,
	item_added_date DATE NOT NULL
)