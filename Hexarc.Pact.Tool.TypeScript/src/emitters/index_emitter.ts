import * as Defs from "./defs";
import * as Syntax from "./syntax";


export function emit(clientClassName: string) {
  return Syntax.createBundle(emitDeclarations(clientClassName));
}

function emitDeclarations(clientClassName: string) {
  return [
    Syntax.createNamedExportDeclaration(Defs.HTTP_ERROR_CLASS_NAME, `./${Defs.HTTP_ERROR_MODULE_PATH}`),
    Syntax.createNamedExportDeclaration(clientClassName, `./${Defs.API_MODULE_PATH}`),
    Syntax.createNamespaceExportDeclaration(Defs.TYPES_NAMESPACE_NAME, `./${Defs.TYPES_MODULE_PATH}`)
  ];
}