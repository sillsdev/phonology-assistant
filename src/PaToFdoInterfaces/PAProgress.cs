// Copyright (c) 2010-2018 SIL International
// License: MIT
using System;
using System.ComponentModel;
using System.Windows.Forms;
using SIL.LCModel.Utils;

namespace SIL.PaToFdoInterfaces
{
    public partial class PAProgress : Form, IThreadedProgress
    {
        private bool _allowCancel;
        private bool _startCancel;
        private bool _finishCancel;

        public PAProgress()
        {
            InitializeComponent();
        }

        public bool Canceled { get { return _finishCancel; } }

        public bool IsCanceling { get { return _startCancel && !_finishCancel; } }

        public string Title { get { return this.Text;  } set { this.Text = value; } }
        public string Message { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public int Position { get { return progressBar1.Value; } set { progressBar1.Value = value; } }
        public int StepSize { get { return progressBar1.Step; } set { progressBar1.Step = value; } }
        public int Minimum { get { return progressBar1.Minimum;  } set { progressBar1.Minimum = value; } }
        public int Maximum { get { return progressBar1.Maximum; } set { progressBar1.Maximum = value; } }

        public ISynchronizeInvoke SynchronizeInvoke
        {
            get
            {
                if (IsDisposed) throw new InvalidOperationException("Progress bar used after disposed");
                return this;
            }
        }

        public bool IsIndeterminate { get { return progressBar1.Style == ProgressBarStyle.Marquee; } set { progressBar1.Style = value ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous; } }
        public bool AllowCancel { get { return _allowCancel; } set { _allowCancel = value; } }

        public event CancelEventHandler Canceling;

        public object RunTask(Func<IThreadedProgress, object[], object> backgroundTask, params object[] parameters)
        {
            return backgroundTask(this, parameters);
        }

        public object RunTask(bool fDisplayUi, Func<IThreadedProgress, object[], object> backgroundTask, params object[] parameters)
        {
            if (fDisplayUi) Hide();
            return backgroundTask(this, parameters);
        }

        public void Step(int amount)
        {
            progressBar1.Value += amount;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            if (!_allowCancel) return;
            _startCancel = true;
            Canceling?.Invoke(this, (CancelEventArgs)e);
            _finishCancel = true;
            DialogResult = DialogResult.Cancel;
        }
    }
}
