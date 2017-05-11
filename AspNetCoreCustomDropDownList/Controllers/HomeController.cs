// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreImageResizingService
{
  public class HomeController : Controller
  {
    [HttpGet]
    public IActionResult Index()
    {
      IndexViewModel indexViewModel = new IndexViewModel();

      indexViewModel.Colors = this.GetColors();
      return this.View(indexViewModel);
    }

    [HttpPost]
    public IActionResult Index(IndexViewModel indexViewModel)
    {
      if (this.ModelState.IsValid)
        return this.Content("Standard color: " + indexViewModel.StandardColor + ", custom color: " + indexViewModel.CustomColor);

      indexViewModel.Colors = this.GetColors();
      return this.View(indexViewModel);
    }

    private IEnumerable<SelectListItem> GetColors()
    {
      return new SelectListItem[]
      {
        new SelectListItem() { Text = "Not set", Value = string.Empty },
        new SelectListItem() { Text = "Red", Value = "red" },
        new SelectListItem() { Text = "Green", Value = "green" },
        new SelectListItem() { Text = "Blue", Value = "blue" }
      };
    }
  }
}