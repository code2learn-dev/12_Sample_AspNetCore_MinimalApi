namespace MinimalApiSample.Models
{
	public class CategoryRepositoy
	{
		private List<Category> _categories = [];

		public IReadOnlyCollection<Category> Categories => _categories;

		public CategoryRepositoy()
		{
			if (_categories.Count == 0)
				_categories = [ new() { Id = 1, Title = "Asp.Net"},
								new() { Id = 2, Title = "C#"},
								new() { Id = 3, Title = ".Net"},
								new() { Id = 4, Title = "Pyton"}];
		}

		public Category? Add(Category category)
		{
			Category? currentCategory = _categories.Find(a => a.Id == category.Id);
			if (currentCategory is not null) return default;
			_categories.Add(category);
			return category;
		}

		public Category? Find(int id) => _categories.Find(a => a.Id == id);

		public Category? Update(Category category)
		{
			Category? categoryToUpdate = _categories.Find(a => a.Id == category.Id);
			if (categoryToUpdate is null) return default;

			categoryToUpdate.Title = category.Title;
			return categoryToUpdate;
		}

		public Category? Delete(int id)
		{
            Category? category = _categories.Find(a => a.Id == id);
			if (category is null) return default;

			_categories.Remove(category);
			return category;
		}
	}
}
