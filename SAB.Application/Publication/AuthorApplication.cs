using SAB.Base.Publication;
using SAB.Domain.Publication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.Publication
{
    public class AuthorApplication
    {
        private readonly IAuthorRepository authorRepository;

        public AuthorApplication(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public IEnumerable<Author> QueryAll()
        {
            IEnumerable<Author> _AuthorList = null;
            try
            {
                _AuthorList = authorRepository.QueryAll();
            }
            catch (Exception)
            {

            }

            return _AuthorList;
        }

        public IEnumerable<Author> Search(int id, string name, string country)
        {
            IEnumerable<Author> _AuthorList = null;
            try
            {
                _AuthorList = authorRepository.Search(id, name, country);;
            }
            catch (Exception)
            {

            }

            return _AuthorList;
        }

        public void Insert(Author author)
        {
            try
            {
                authorRepository.Insert(author);
            }
            catch (Exception)
            {

            }
        }

        public Author QueryById(int id)
        {
            Author _Author = null;
            try
            {
                _Author = authorRepository.QueryById(id);
            }
            catch (Exception)
            {

            }

            return _Author;
        }

        public void Update(Author author)
        {
            try
            {
                authorRepository.Update(author);
            }
            catch (Exception)
            {

            }
        }
    }
}
