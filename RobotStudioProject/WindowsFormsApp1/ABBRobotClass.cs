using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABB.Robotics;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;
using Task = ABB.Robotics.Controllers.RapidDomain.Task;

namespace WindowsFormsApp1
{
    class ABBRobotClass
    {
        private Controller controller = null;
        private RapidData rd_start = null;
        private const int SHUT_DOWN = -1;
        private Num processFlag;

        public ABBRobotClass(Controller controller)
        {
            this.controller = controller;
            this.controller.Logon(UserInfo.DefaultUser);
            InitDataStream();
        }

        private void InitDataStream()
        {
            Task tRob1 = controller.Rapid.GetTask("T_ROB1");
            if (tRob1 != null)
            { //MessageBox.Show("Congratulations!");
            }
        }



        #region HelperMethods
        public void StopProcess()
        {
            //repeatBool.FillFromString2("FALSE");
            processFlag.FillFromString2(SHUT_DOWN.ToString());
            using (Mastership m = Mastership.Request(controller.Rapid))
            {
                //rd_repeat.Value = repeatBool;
                rd_start.Value = processFlag;
            }
        }
        #endregion

        #region Controller
        public Controller Controller
        {
            get
            {
                return controller;
            }
        }
        #endregion

        public void Dispose()
        {
            if (controller.Rapid.ExecutionStatus == ExecutionStatus.Running)
            {
                StopProcess();
            }
            this.controller.Logoff();
            this.controller.Dispose();
            this.controller = null;
        }
    }
}
