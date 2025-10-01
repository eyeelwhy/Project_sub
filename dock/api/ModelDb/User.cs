using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace api_docker.ModelDb;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int IdRole { get; set; }

    [JsonIgnore]
    public virtual Role IdRoleNavigation { get; set; } = null!;
}
