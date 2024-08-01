using ADO_09._Dapper_ORM;

using Dapper;

using System.Data;

public class AuthorRepository : IAuthorRepository
{
    private IDbConnection _db;
    public AuthorRepository(IDbConnection db, string connectionString)
    {
        _db = db;
        _db.ConnectionString = connectionString;
    }
    public Author Add(Author author)
    {
        var sql =
@"INSERT INTO Authors(FirstName, LastName)
VALUES(@FirstName, @LastName)
SELECT CAST(SCOPE_IDENTITY() AS int)";
        var id = _db.Query<int>(sql,
            new
            {
                @FirstName = author.FirstName,
                @LastName = author.LastName
            }).FirstOrDefault();
        author.Id = id;
        return author;

    }

    public void AddAuthors(List<Author> authors)
    {
        string sql;
        foreach (var author in authors)
        {
            sql =
                @"INSERT INTO Authors(FirstName, LastName) 
                VALUES(@FirstName,@LastName)";
            _db.Query(sql, new { @FirstName = author.FirstName, @LastName = author.LastName });
        }
    }

    public IEnumerable<Author> GetAll()
    {
        var sql = "SELECT * FROM Authors";
        return _db.Query<Author>(sql);
    }

    public Author GetById(int id)
    {
        return _db.Query<Author>("SELECT * FROM Authors WHERE Id=@Id", new { @Id = id })
            .FirstOrDefault()!;
    }

    public void Remove(int id)
    {
        _db.Execute("DELETE FROM Authors WHERE Id=@Id", new { @Id = id });
    }

    public void RemoveByIdArray(int[] ids)
    {
        foreach (int id in ids)
        {
            _db.Execute("DELETE FROM Authors WHERE Id=@Id", new { @Id = id });
        }

    }

    public void RemoveRange(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            _db.Execute("DELETE FROM Authors WHERE Id=@Id", new { @Id = i });
        }
    }

    public Author Update(Author author)
    {
        var sql = "UPDATE Authors SET FirstName =@FirstName, LastName=@LastName WHERE Id=@Id";
        _db.Execute(sql, author);
        return author;
    }

    public void UpdateAuthors(List<Author> authors)
    {
        foreach (Author author in authors)
        {
            var sql = "UPDATE Authors SET FirstName =@FirstName, LastName=@LastName WHERE Id=@Id";
            _db.Execute(sql, author);
        }
    }
}