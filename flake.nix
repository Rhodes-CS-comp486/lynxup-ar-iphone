{
  description = "Python Dev Environment with Flask";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }: flake-utils.lib.eachDefaultSystem (system:
    let
      pkgs = import nixpkgs { inherit system; };
    in
    {
      devShell = pkgs.mkShell {
        name = "python-dev";

        buildInputs = [
          pkgs.python3
          pkgs.python3Packages.pip
          pkgs.python3Packages.virtualenv
          pkgs.python3Packages.flask
        ];

        nativeBuildInputs = [
          pkgs.git
          pkgs.direnv
          pkgs.jq
        ];

        # Write custom config.fish to a temporary file and set XDG_CONFIG_HOME to point to it
        shellHook = ''
           export PS1="\n($name) $PS1"
        '';
      };
    });
}
