-- Lab 07, example 3 «Довідник товарів».

CREATE TABLE products (
    id       integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name     varchar(100) NOT NULL,
    category varchar(50) NOT NULL,
    price    numeric(10, 2) NOT NULL CHECK (price >= 0),
    stock    integer NOT NULL DEFAULT 0 CHECK (stock >= 0)
);

CREATE TABLE stock_moves (
    id         bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    product_id integer NOT NULL REFERENCES products (id),
    quantity   integer NOT NULL CHECK (quantity <> 0),
    moved_at   timestamptz NOT NULL DEFAULT now()
);

INSERT INTO products (name, category, price, stock)
VALUES ('USB cable', 'Accessories', 149.90, 40),
       ('Wireless mouse', 'Accessories', 499.00, 12),
       ('Laptop 15 inch', 'Computers', 32999.00, 3),
       ('Monitor 27 inch', 'Computers', 8999.00, 5);
