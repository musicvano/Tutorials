-- Lab 07, example 1 «Спортивний клуб»: tables, data and queries 1–5.

CREATE TABLE trainers (
    id    integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name  varchar(100) NOT NULL,
    phone varchar(20) UNIQUE
);

CREATE TABLE sections (
    id         integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name       varchar(50) NOT NULL UNIQUE,
    trainer_id integer REFERENCES trainers (id) ON DELETE SET NULL,
    fee        numeric(8, 2) NOT NULL CHECK (fee > 0)
);

CREATE TABLE members (
    id        integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name      varchar(100) NOT NULL,
    birth     date NOT NULL,
    joined_at date NOT NULL DEFAULT CURRENT_DATE
);

CREATE TABLE enrollments (
    member_id  integer REFERENCES members (id) ON DELETE CASCADE,
    section_id integer REFERENCES sections (id) ON DELETE CASCADE,
    PRIMARY KEY (member_id, section_id)
);

CREATE TABLE payments (
    id        bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    member_id integer NOT NULL REFERENCES members (id),
    amount    numeric(8, 2) NOT NULL CHECK (amount > 0),
    paid_on   date NOT NULL
);

INSERT INTO trainers (name, phone)
VALUES ('Andrii Melnyk', '+380501112233'),
       ('Oksana Tkachenko', '+380671234567'),
       ('Serhii Lysenko', NULL);

INSERT INTO sections (name, trainer_id, fee)
VALUES ('Swimming', 1, 900), ('Tennis', 2, 1200),
       ('Yoga', 2, 700), ('Boxing', NULL, 1000);

INSERT INTO members (name, birth, joined_at)
VALUES ('Denys Koval', '2004-03-15', '2026-01-10'),
       ('Mariia Savchuk', '2006-07-02', '2026-02-01'),
       ('Taras Oliinyk', '1999-11-20', '2026-02-15'),
       ('Yuliia Moroz', '2008-05-09', '2026-03-03');

INSERT INTO enrollments (member_id, section_id)
VALUES (1, 1), (1, 2), (2, 3), (3, 1), (3, 3), (4, 3);

INSERT INTO payments (member_id, amount, paid_on)
VALUES (1, 2100, '2026-01-10'), (2, 700, '2026-02-01'),
       (3, 1600, '2026-02-15'), (1, 2100, '2026-02-10'),
       (2, 700, '2026-03-01'), (3, 1600, '2026-03-15');

-- 1. Секції з тренерами (секція без тренера теж).
SELECT s.name AS section, t.name AS trainer, s.fee
FROM sections AS s
LEFT JOIN trainers AS t ON t.id = s.trainer_id
ORDER BY s.name;

-- 2. Кількість учасників у кожній секції.
SELECT s.name AS section, count(e.member_id) AS members
FROM sections AS s
LEFT JOIN enrollments AS e ON e.section_id = s.id
GROUP BY s.id, s.name
ORDER BY members DESC, s.name;

-- 3. Надходження за місяцями.
SELECT to_char(paid_on, 'YYYY-MM') AS month,
       count(*) AS payments, sum(amount) AS total
FROM payments
GROUP BY month
ORDER BY month;

-- 4. Щомісячна плата учасника понад 1000 грн.
SELECT m.name, sum(s.fee) AS monthly_fee
FROM members AS m
JOIN enrollments AS e ON e.member_id = m.id
JOIN sections AS s ON s.id = e.section_id
GROUP BY m.id, m.name
HAVING sum(s.fee) > 1000
ORDER BY monthly_fee DESC;

-- 5. Учасники, які не платили в березні 2026 року.
SELECT name
FROM members AS m
WHERE NOT EXISTS (
    SELECT 1 FROM payments AS p
    WHERE p.member_id = m.id
      AND p.paid_on >= DATE '2026-03-01'
      AND p.paid_on < DATE '2026-04-01')
ORDER BY name;

UPDATE sections SET fee = round(fee * 1.1, 2) WHERE trainer_id IS NULL RETURNING name, fee;
