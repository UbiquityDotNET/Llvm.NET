import { Kaleidoscope } from './Kaleidoscope.js'
import { antlr } from './antlr.js'

export default
{
  configureHljs (hljs)
  {
    hljs.registerLanguage('Kaleidoscope', Kaleidoscope);
    hljs.registerLanguage('antlr', antlr);
  },
}
