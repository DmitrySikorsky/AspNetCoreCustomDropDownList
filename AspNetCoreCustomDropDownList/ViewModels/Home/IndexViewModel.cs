// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreCustomDropDownList
{
  public class IndexViewModel
  {
    [Required]
    public string StandardColor { get; set; }

    [Required]
    public string CustomColor { get; set; }
    public IEnumerable<SelectListItem> Colors { get; set; }
  }
}