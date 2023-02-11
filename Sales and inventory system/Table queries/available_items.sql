CREATE TABLE available_items(
	available_item_id INT PRIMARY KEY IDENTITY(1,1),
	item_id INT NOT NULL FOREIGN KEY REFERENCES items(item_id),
	item_stock INT NOT NULL,
	stock_copy INT NOT NULL
)