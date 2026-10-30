using WarehouseAPI.DTOs;

namespace WarehouseAPI.Validators
{
    public class ProductValidator
    {
        public bool IsNameValid(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public bool IsPriceValid(decimal price)
        {
            return price > 0;
        }

        public bool IsQuantityValid(int quantity)
        {
            return quantity >= 0;
        }

        public bool IsProductValid(CreatedProductDTO model)
        {
            return model != null
                && IsNameValid(model.Name)
                && IsPriceValid(model.Price)
                && IsQuantityValid(model.Quantity);
        }
    }
}
