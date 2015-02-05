using SAB.Base.User;
using SAB.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Application.User
{
    public class ActionTypeApplication
    {
        private readonly IActionRepository actionRepository;

        public ActionTypeApplication(IActionRepository actionRepository)
        {
            this.actionRepository = actionRepository;
        }

        public List<ActionType> QueryAllByIdPerfil(int id)
        {
            List<ActionType> _actionTypeList = null;
            try
            {
                _actionTypeList = actionRepository.QueryAllByIdPerfil(id);
            }
            catch (Exception) { 
            
            }
            return _actionTypeList;
        }




        public IEnumerable<ActionType> QueryAll() {

            IEnumerable<ActionType> _actionTypeList = null;
            try
            {
                _actionTypeList = actionRepository.QueryAll();
            }
            catch (Exception)
            {

            }
            return _actionTypeList;
        }

        public void DeleteAllByIdPerfil(int id)
        {

          
            try
            {
                actionRepository.DeleteById(id);
            }
            catch (Exception)
            {

            }
       
        }
        
        
    }
}
