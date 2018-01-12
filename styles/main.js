// Use container fluid
var containers = $(".container");
containers.removeClass("container");
containers.addClass("container-fluid");

// Navbar Hamburger
$(function () {
    $(".navbar-toggle").click(function () {
        $(this).toggleClass("change");
    })
})

// Select list to replace affix on small screens
$(function () {
    var navItems = $(".sideaffix .level1 > li");

    if (navItems.length == 0) {
        return;
    }

    var selector = $("<select/>");
    selector.addClass("form-control visible-sm visible-xs");
    var form = $("<form/>");
    form.append(selector);
    form.prependTo("article");

    selector.change(function () {
        window.location = $(this).find("option:selected").val();
    })

    function work(item, level) {
        var link = item.children('a');

        var text = link.text();

        for (var i = 0; i < level; ++i) {
            text = '&nbsp;&nbsp;' + text;
        }

        selector.append($('<option/>', {
            'value': link.attr('href'),
            'html': text
        }));

        var nested = item.children('ul');

        if (nested.length > 0) {
            nested.children('li').each(function () {
                work($(this), level + 1);
            });
        }
    }

    navItems.each(function () {
        work($(this), 0);
    });
})
// Add EBNF language from highlightjs.org
hljs.registerLanguage("ebnf", function (a) {
    var e = a.C(/\(\*/, /\*\)/),
        t = {
            cN: "attribute",
            b: /^[ ]*[a-zA-Z][a-zA-Z-]*([\s-]+[a-zA-Z][a-zA-Z]*)*/
        },
        r = {
            cN: "meta",
            b: /\?.*\?/
        },
        b = {
            b: /=/,
            e: /;/,
            c: [e, r, a.ASM, a.QSM]
        };
    return {
        i: /\S/,
        c: [e, t, b]
    }
});
// Add LLVM IR language support from highlightjs.org
hljs.registerLanguage("llvm", function (e) {
    var n = "([-a-zA-Z$._][\\w\\-$.]*)";
    return {
        k: "begin end true false declare define global constant private linker_private internal available_externally linkonce linkonce_odr weak weak_odr appending dllimport dllexport common default hidden protected extern_weak external thread_local zeroinitializer undef null to tail target triple datalayout volatile nuw nsw nnan ninf nsz arcp fast exact inbounds align addrspace section alias module asm sideeffect gc dbg linker_private_weak attributes blockaddress initialexec localdynamic localexec prefix unnamed_addr ccc fastcc coldcc x86_stdcallcc x86_fastcallcc arm_apcscc arm_aapcscc arm_aapcs_vfpcc ptx_device ptx_kernel intel_ocl_bicc msp430_intrcc spir_func spir_kernel x86_64_sysvcc x86_64_win64cc x86_thiscallcc cc c signext zeroext inreg sret nounwind noreturn noalias nocapture byval nest readnone readonly inlinehint noinline alwaysinline optsize ssp sspreq noredzone noimplicitfloat naked builtin cold nobuiltin noduplicate nonlazybind optnone returns_twice sanitize_address sanitize_memory sanitize_thread sspstrong uwtable returned type opaque eq ne slt sgt sle sge ult ugt ule uge oeq one olt ogt ole oge ord uno ueq une x acq_rel acquire alignstack atomic catch cleanup filter inteldialect max min monotonic nand personality release seq_cst singlethread umax umin unordered xchg add fadd sub fsub mul fmul udiv sdiv fdiv urem srem frem shl lshr ashr and or xor icmp fcmp phi call trunc zext sext fptrunc fpext uitofp sitofp fptoui fptosi inttoptr ptrtoint bitcast addrspacecast select va_arg ret br switch invoke unwind unreachable indirectbr landingpad resume malloc alloca free load store getelementptr extractelement insertelement shufflevector getresult extractvalue insertvalue atomicrmw cmpxchg fence argmemonly double distinct",
        c: [{
            cN: "keyword",
            b: "i\\d+"
        }, e.C(";", "\\n", {
            r: 0
        }), e.QSM, {
            cN: "string",
            v: [{
                b: '"',
                e: '[^\\\\]"'
            }],
            r: 0
        }, {
            cN: "title",
            v: [{
                b: "@" + n
            }, {
                b: "@\\d+"
            }, {
                b: "!" + n
            }, {
                b: "!\\d+" + n
            }]
        }, {
            cN: "symbol",
            v: [{
                b: "%" + n
            }, {
                b: "%\\d+"
            }, {
                b: "#\\d+"
            }]
        }, {
            cN: "number",
            v: [{
                b: "0[xX][a-fA-F0-9]+"
            }, {
                b: "-?\\d+(?:[.]\\d+)?(?:[eE][-+]?\\d+(?:[.]\\d+)?)?"
            }],
            r: 0
        }]
    }
});
// Custom ANTLR language support
hljs.registerLanguage("antlr", function (e) {
    return {
        k:
          'grammar fragment',
        c: [
          e.C(
            '//', '\\n', { r: 0 }
          ),
          e.QSM,
          e.ASM,
          {
              cN: 'meta',
              v: [
                { b: '{', e: '}' },
                { b: '{', e: '}\?' },
              ],
              r: 0
          },
          {
              cN: 'symbol',
              v: [
                { b: '([A-Z][\\w\\-$.]*)' },
              ]
          },
          {
              cN: 'number',
              v: [
                  { b: '0[xX][a-fA-F0-9]+' },
                  { b: '-?\\d+(?:[.]\\d+)?(?:[eE][-+]?\\d+(?:[.]\\d+)?)?' }
              ],
              r: 0
          },
          {
              cN: 'title',
              v: [
                { b: '([-a-zA-Z$._][\\w\\-$.]*)' },
              ]
          },

        ]
    };
}
);

