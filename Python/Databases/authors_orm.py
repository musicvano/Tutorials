# authors_orm.py
import sqlite3
from sqlalchemy import ForeignKey, create_engine, event, select
from sqlalchemy.orm import DeclarativeBase, Mapped, Session
from sqlalchemy.orm import mapped_column, relationship, selectinload
from sqlalchemy.pool import ConnectionPoolEntry


class Base(DeclarativeBase):
    pass


class Author(Base):
    __tablename__ = "authors"
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str]
    books: Mapped[list["Book"]] = relationship(
        back_populates="author", cascade="all, delete-orphan")


class Book(Base):
    __tablename__ = "books"
    id: Mapped[int] = mapped_column(primary_key=True)
    title: Mapped[str]
    author_id: Mapped[int] = mapped_column(ForeignKey("authors.id"))
    author: Mapped[Author] = relationship(back_populates="books")


engine = create_engine("sqlite://",
                       connect_args={"autocommit": False})


@event.listens_for(engine, "connect")
def foreign_keys(db: sqlite3.Connection,
                 record: ConnectionPoolEntry) -> None:
    previous = db.autocommit
    db.autocommit = True
    cursor = db.execute("PRAGMA foreign_keys=ON")
    cursor.close()
    db.autocommit = previous


Base.metadata.create_all(engine)
with Session(engine) as session:
    with session.begin():
        author = Author(name="Леся")
        author.books = [Book(title="Ліс"), Book(title="Місто")]
        session.add(author)
    query = select(Author).options(selectinload(Author.books))
    for author in session.scalars(query.order_by(Author.id)):
        titles = sorted(book.title for book in author.books)
        print(author.name, ", ".join(titles))
engine.dispose()
