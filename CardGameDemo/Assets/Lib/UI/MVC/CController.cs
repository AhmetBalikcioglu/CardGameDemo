namespace Lib.Mvc
{
    public abstract class CController<TModel, TView>
        where TModel : CModel
        where TView : CView<TModel>
    {
        #region Model

        protected TModel _model;
        public TModel Model => _model;

        #endregion

        #region View

        protected TView _view;
        public TView View => _view;

        #endregion


        #region Init | DeInit

        public void Init(TModel model, TView view)
        {
            _model = model;
            _view = view;
        }

        public void DeInit()
        {
            Hide();

            _model = null;
            _view = null;
        }

        #endregion


        #region Model: Set

        public void SetModel(TModel model)
        {
            _model = model;
            _view.SetModel(_model);
        }

        #endregion


        #region Show -> OnShow

        public void Show()
        {
            _view.SetModel(_model);
            _view.Show();

            OnShow();

            OnAddEvents();
        }

        protected abstract void OnShow();

        #endregion

        #region Hide -> OnHide

        public void Hide()
        {
            OnRemoveEvents();

            _view.Hide();

            OnHide();
        }

        protected abstract void OnHide();

        #endregion


        #region Events: OnAdd | OnRemove

        protected abstract void OnAddEvents();
        protected abstract void OnRemoveEvents();

        #endregion
    }
}